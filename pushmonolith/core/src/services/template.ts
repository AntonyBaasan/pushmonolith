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
      "Default" : "ec2_login_key_pair",
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
    "SSHLocation" : {
      "Description" : "The IP address range that can be used to SSH to the EC2 instances",
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
      "t1.micro": { "Arch": "HVM64" },
      "t2.nano": { "Arch": "HVM64" },
      "t2.micro": { "Arch": "HVM64" },
      "t2.small": { "Arch": "HVM64" },
      "t2.medium": { "Arch": "HVM64" },
      "t2.large": { "Arch": "HVM64" },
      "m1.small": { "Arch": "HVM64" },
      "m1.medium": { "Arch": "HVM64" },
      "m1.large": { "Arch": "HVM64" },
      "m1.xlarge": { "Arch": "HVM64" },
      "m2.xlarge": { "Arch": "HVM64" },
      "m2.2xlarge": { "Arch": "HVM64" },
      "m2.4xlarge": { "Arch": "HVM64" },
      "m3.medium": { "Arch": "HVM64" },
      "m3.large": { "Arch": "HVM64" },
      "m3.xlarge": { "Arch": "HVM64" },
      "m3.2xlarge": { "Arch": "HVM64" },
      "m4.large": { "Arch": "HVM64" },
      "m4.xlarge": { "Arch": "HVM64" },
      "m4.2xlarge": { "Arch": "HVM64" },
      "m4.4xlarge": { "Arch": "HVM64" },
      "m4.10xlarge": { "Arch": "HVM64" },
      "c1.medium": { "Arch": "HVM64" },
      "c1.xlarge": { "Arch": "HVM64" },
      "c3.large": { "Arch": "HVM64" },
      "c3.xlarge": { "Arch": "HVM64" },
      "c3.2xlarge": { "Arch": "HVM64" },
      "c3.4xlarge": { "Arch": "HVM64" },
      "c3.8xlarge": { "Arch": "HVM64" },
      "c4.large": { "Arch": "HVM64" },
      "c4.xlarge": { "Arch": "HVM64" },
      "c4.2xlarge": { "Arch": "HVM64" },
      "c4.4xlarge": { "Arch": "HVM64" },
      "c4.8xlarge": { "Arch": "HVM64" },
      "g2.2xlarge": { "Arch": "HVMG2" },
      "g2.8xlarge": { "Arch": "HVMG2" },
      "r3.large": { "Arch": "HVM64" },
      "r3.xlarge": { "Arch": "HVM64" },
      "r3.2xlarge": { "Arch": "HVM64" },
      "r3.4xlarge": { "Arch": "HVM64" },
      "r3.8xlarge": { "Arch": "HVM64" },
      "i2.xlarge": { "Arch": "HVM64" },
      "i2.2xlarge": { "Arch": "HVM64" },
      "i2.4xlarge": { "Arch": "HVM64" },
      "i2.8xlarge": { "Arch": "HVM64" },
      "d2.xlarge": { "Arch": "HVM64" },
      "d2.2xlarge": { "Arch": "HVM64" },
      "d2.4xlarge": { "Arch": "HVM64" },
      "d2.8xlarge": { "Arch": "HVM64" },
      "hi1.4xlarge": { "Arch": "HVM64" },
      "hs1.8xlarge": { "Arch": "HVM64" },
      "cr1.8xlarge": { "Arch": "HVM64" },
      "cc2.8xlarge": { "Arch": "HVM64" }
    },

    "AWSRegionArch2AMI": {
      "us-east-1": { "HVM64": "ami-0ff8a91507f77f867", "HVMG2": "ami-0a584ac55a7631c0c" },
      "us-west-2": { "HVM64": "ami-a0cfeed8", "HVMG2": "ami-0e09505bc235aa82d" },
      "us-west-1": { "HVM64": "ami-0bdb828fd58c52235", "HVMG2": "ami-066ee5fd4a9ef77f1" },
      "eu-west-1": { "HVM64": "ami-047bb4163c506cd98", "HVMG2": "ami-0a7c483d527806435" },
      "eu-west-2": { "HVM64": "ami-f976839e", "HVMG2": "NOT_SUPPORTED" },
      "eu-west-3": { "HVM64": "ami-0ebc281c20e89ba4b", "HVMG2": "NOT_SUPPORTED" },
      "eu-central-1": { "HVM64": "ami-0233214e13e500f77", "HVMG2": "ami-06223d46a6d0661c7" },
      "ap-northeast-1": { "HVM64": "ami-06cd52961ce9f0d85", "HVMG2": "ami-053cdd503598e4a9d" },
      "ap-northeast-2": { "HVM64": "ami-0a10b2721688ce9d2", "HVMG2": "NOT_SUPPORTED" },
      "ap-northeast-3": { "HVM64": "ami-0d98120a9fb693f07", "HVMG2": "NOT_SUPPORTED" },
      "ap-southeast-1": { "HVM64": "ami-08569b978cc4dfa10", "HVMG2": "ami-0be9df32ae9f92309" },
      "ap-southeast-2": { "HVM64": "ami-09b42976632b27e9b", "HVMG2": "ami-0a9ce9fecc3d1daf8" },
      "ap-south-1": { "HVM64": "ami-0912f71e06545ad88", "HVMG2": "ami-097b15e89dbdcfcf4" },
      "us-east-2": { "HVM64": "ami-0b59bfac6be064b78", "HVMG2": "NOT_SUPPORTED" },
      "ca-central-1": { "HVM64": "ami-0b18956f", "HVMG2": "NOT_SUPPORTED" },
      "sa-east-1": { "HVM64": "ami-07b14488da8ea02a0", "HVMG2": "NOT_SUPPORTED" },
      "cn-north-1": { "HVM64": "ami-0a4eaf6c4454eda75", "HVMG2": "NOT_SUPPORTED" },
      "cn-northwest-1": { "HVM64": "ami-6b6a7d09", "HVMG2": "NOT_SUPPORTED" }
    }
  },

  // "Conditions": {
  //   set of conditions
  // },

  // "Transform": {
  //   set of transforms
  // },

  "Resources": {

    "WebServerInstance": {
      "Type": "AWS::EC2::Instance",
      "Properties": {
        "ImageId": {
          "Fn::FindInMap": ["AWSRegionArch2AMI", { "Ref": "AWS::Region" },
            { "Fn::FindInMap": ["AWSInstanceType2Arch", { "Ref": "InstanceType" }, "Arch"] }]
        },
        "InstanceType": { "Ref": "InstanceType" },
        "SecurityGroups": [{ "Ref": "WebServerSecurityGroup" }],
        "KeyName": { "Ref": "KeyName" }
      }
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
