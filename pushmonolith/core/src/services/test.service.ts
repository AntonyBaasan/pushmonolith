import {
    CloudFormationClientConfig,
    CloudFormation,
    CreateStackCommandInput,
    Stack,
    Capability
} from '@aws-sdk/client-cloudformation';
import { S3Service } from './s3.service';
import { BaseTemplate } from './template';

export class TestService {
    public cloudformation: CloudFormation;
    public s3Service: S3Service;

    constructor() {
        this.s3Service = new S3Service();
        const config: CloudFormationClientConfig = {
            region: 'us-east-1'
        };
        this.cloudformation = new CloudFormation(config)
    }

    public async saySomething2(): Promise<string> {
        await this.s3Service.createBucket('C:\\Users\\abaasandorj\\git\\pushmonolith\\examples\\spring\\webapi\\target\\webapi-0.0.1-SNAPSHOT.jar');
        return 'Done';
    }

    public async saySomething(): Promise<string> {

        const createStackParam: CreateStackCommandInput = this.createEc2StackInput();

        const stack: Stack = await this.getStackInfo(createStackParam.StackName);
        // console.log(stack);

        try {
            let data = null;
            if (stack.StackId) {
                console.log('updating stack ...');
                data = await this.cloudformation.updateStack(createStackParam);
            } else {
                console.log('creating stack ...');
                data = await this.cloudformation.createStack(createStackParam);
            }
            return JSON.stringify(data);
        } catch (error) {
            return JSON.stringify(error);
        }

    }

    private async getStackInfo(stackName: string | undefined): Promise<Stack> {
        try {
            if (stackName) {
                const describe = await this.cloudformation.describeStacks({ StackName: stackName });
                if (describe.Stacks && describe.Stacks.length > 0) {
                    return describe.Stacks[0];
                }
            }
        } catch (error) {
            console.log('can\'t find the stack');
            // console.log(error);
        }
        return {} as Stack;
    }

    private createEc2StackInput(): CreateStackCommandInput {
        return {
            StackName: 'PushMonolith',
            OnFailure: 'ROLLBACK',
            Tags: [
                {
                    Key: 'App',
                    Value: 'Pushmonolith'
                },
            ],
            TemplateBody: JSON.stringify(BaseTemplate),
            Capabilities: [Capability.CAPABILITY_IAM]
        } as CreateStackCommandInput;
    }
}