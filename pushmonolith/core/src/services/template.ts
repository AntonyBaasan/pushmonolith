export const BaseTemplate = {
  "AWSTemplateFormatVersion": "2010-09-09",
  "Description": "Demo template",
  // "Metadata": {
  //   "Instances": { "Description": "Information about the instances" },
  // },

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
      "t1.micro": { "Arch": "Ubuntu" },
      "t2.nano": { "Arch": "Ubuntu" },
      "t2.micro": { "Arch": "Ubuntu" },
      "t2.small": { "Arch": "Ubuntu" },
    },

    "AWSRegionArch2AMI": {
      "us-east-1": { "Ubuntu": "ami-0b0ea68c435eb488d" },
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
          "files": {
            "/var/pushmonolith/app.jar": {
              "source": "s3://pushmonolith-2/app.jar"
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
