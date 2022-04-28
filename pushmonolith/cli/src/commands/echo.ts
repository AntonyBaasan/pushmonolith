import { Command, Flags } from '@oclif/core'
import { TestService } from '@pushmonolith/core'

export default class Echo extends Command {
  static description = 'describe the command here'

  static examples = [
    '<%= config.bin %> <%= command.id %>',
  ]

  // static flags = {
  //   // flag with a value (-n, --name=VALUE)
  //   name: Flags.string({ char: 'n', description: 'name to print' }),
  //   // flag with no value (-f, --force)
  //   force: Flags.boolean({ char: 'f' }),
  // }

  // static args = [{ name: 'file' }]

  public async run(): Promise<void> {
    // const { args, flags } = await this.parse(Echo)
    // const name = flags.name ?? 'world'
    // this.log(`hello ${name} from C:\\Users\\abaasandorj\\git\\pushmonolith\\pushmonolith\\cli\\src\\commands\\echo.ts`)

    const service = new TestService()
    const result = await service.saySomething()
    console.log(result)
  }
}
