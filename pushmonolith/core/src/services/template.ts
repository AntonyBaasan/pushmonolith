export const BaseTemplate = {
  "AWSTemplateFormatVersion": "2010-09-09",
  "Description": "Demo template",
  "Metadata": {
    "Instances": { "Description": "Information about the instances" },
  },

  "Parameters": {
    "KeyName": {
      "Description": "Name of an existing EC2 KeyPair to enable SSH access to the instance",
      "Type": "AWS::EC2::KeyPair::KeyName",
      // "ConstraintDescription": "Can contain only ASCII characters.",
      "Default": "ec2_login_key_pair",
    },
    "InstanceType": {
      "Description": "WebServer EC2 instance type",
      "Type": "String",
      "Default": "t2.micro",
      "AllowedValues": [
        "t2.micro",
        "t3.micro",
      ],
      "ConstraintDescription": "must be a valid EC2 instance type."
    },
    "SSHLocation": {
      "Description": "The IP address range that can be used to SSH to the EC2 instances",
      "Type": "String",
      "MinLength": "9",
      "MaxLength": "18",
      "Default": "0.0.0.0/0",
      "AllowedPattern": "(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})/(\\d{1,2})",
      "ConstraintDescription": "Must be a valid IP CIDR range of the form x.x.x.x/x"
    }
  },

  // "Rules": {
  //   set of rules
  // },

  "Mappings": {
    "AWSInstanceType2Arch": {
      "t1.micro": { "Arch": "Amaz" },
      "t2.nano": { "Arch": "Amaz" },
      "t2.micro": { "Arch": "Amaz" },
      "t2.small": { "Arch": "Amaz" },
    },

    "AWSRegionArch2AMI": {
      "us-east-1": { "Ubuntu": "ami-0b0ea68c435eb488d", "Amaz": "ami-0022f774911c1d690" },
    }
  },

  // "Conditions": {
  //   set of conditions
  // },

  // "Transform": {
  //   set of transforms
  // },

  "Resources": {
    "InstanceRole": {
      "Type": "AWS::IAM::Role",
      "Properties": {
        "AssumeRolePolicyDocument": {
          "Statement": [
            {
              "Effect": "Allow",
              "Principal": {
                "Service": [
                  "ec2.amazonaws.com"
                ]
              },
              "Action": [
                "sts:AssumeRole"
              ]
            }
          ]
        },
        "Path": "/"
      }
    },
    "RolePolicies": {
      "Type": "AWS::IAM::Policy",
      "Properties": {
        "PolicyName": "S3Download",
        "PolicyDocument": {
          "Statement": [
            {
              "Action": [
                "s3:GetObject"
              ],
              "Effect": "Allow",
              "Resource": "arn:aws:s3:::pushmonolith-2/app.jar"
            }
          ]
        },
        "Roles": [
          {
            "Ref": "InstanceRole"
          }
        ]
      }
    },
    "InstanceProfile": {
      "Type": "AWS::IAM::InstanceProfile",
      "Properties": {
        "Path": "/",
        "Roles": [
          {
            "Ref": "InstanceRole"
          }
        ]
      }
    },
    "WebServerInstance": {
      "Type": "AWS::EC2::Instance",
      "Metadata": {
        "AWS::CloudFormation::Authentication": {
          "S3AccessCreds": {
            "type": "S3",
            "roleName": {
              "Ref": "InstanceRole"
            }
          }
        },
        "AWS::CloudFormation::Init": {
          "configSets": {
            "Install": [
              "Install"
            ]
          },
          "Install": {
            "packages": {
            },
            "files": {
              "/var/pushmonolith/app.jar": {
                "source": "https://pushmonolith-2.s3.amazonaws.com/app.jar",
                "mode": "000400",
                "owner": "ubuntu",
                "group": "ubuntu",
                "authentication": "S3AccessCreds"
              },
              "/etc/nginx/conf.d/default.conf": {
                "content": {
                  "Fn::Join": ["", [
                    "server { \n",
                    "  listen 80; \n",

                    "  location / { \n",
                    "      proxy_set_header   X-Forwarded-For $remote_addr; \n",
                    "      proxy_set_header   Host $http_host; \n",
                    "      proxy_pass         \"http://127.0.0.1:8080\"; \n",
                    "  } \n",
                    "} \n",
                  ]]
                },
                "mode": "000644",
                "owner": "root",
                "group": "root"
              }
            }
          },
        },
      },
      "Properties": {
        "ImageId": {
          "Fn::FindInMap": ["AWSRegionArch2AMI", { "Ref": "AWS::Region" },
            { "Fn::FindInMap": ["AWSInstanceType2Arch", { "Ref": "InstanceType" }, "Arch"] }]
        },
        "InstanceType": { "Ref": "InstanceType" },
        "SecurityGroups": [{ "Ref": "WebServerSecurityGroup" }],
        "KeyName": { "Ref": "KeyName" },
        "IamInstanceProfile": {
          "Ref": "InstanceProfile"
        },
        "UserData": {
          "Fn::Base64": {
            "Fn::Join": ["", [
              "#!/bin/bash -xe\n",
              "amazon-linux-extras install nginx1 \n",
              "amazon-linux-extras install java-openjdk11 \n",
              "yum install -y aws-cfn-bootstrap\n",
              "# Install the files and packages from the metadata\n",
              "/opt/aws/bin/cfn-init -v ",
              "         --stack ",
              {
                "Ref": "AWS::StackName"
              },
              "         --resource WebServerInstance ",
              "         --configsets Install ",
              "         --region ",
              {
                "Ref": "AWS::Region"
              },
              "\n",
              "# Start nginx\n",
              "systemctl start nginx \n",
              "# Start Jar application \n",
              "ln -s /var/pushmonolith/app.jar /etc/init.d/pushmonolith",
              "service pushmonolith start",
              "\n"
            ]]
          }
        }
      },
    },

    "WebServerSecurityGroup": {
      "Type": "AWS::EC2::SecurityGroup",
      "Properties": {
        "GroupDescription": "Enable HTTP access via port 80",
        "SecurityGroupIngress": [
          { "IpProtocol": "tcp", "FromPort": 80, "ToPort": 80, "CidrIp": "0.0.0.0/0" },
          { "IpProtocol": "tcp", "FromPort": 22, "ToPort": 22, "CidrIp": { "Ref": "SSHLocation" } }
        ]
      }
    }
  },

  "Outputs": {
    "WebsiteURL": {
      "Description": "URL for newly created LAMP stack",
      "Value": { "Fn::Join": ["", ["http://", { "Fn::GetAtt": ["WebServerInstance", "PublicDnsName"] }]] }
    }
  }
}
