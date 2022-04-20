import {Command, Flags} from '@oclif/core'
import {User} from '@pushmonolith/models'
import {TestService} from '@pushmonolith/core'
// import * as Models from '@pushmonolith/models'

export default class Deploy extends Command {
  static description = 'deploys artifact into cloud providers'

  static examples = [
    '<%= config.bin %> <%= command.id %>',
  ]

  static flags = {
    // flag with a value (-n, --name=VALUE)
    file: Flags.string({char: 'f', description: 'artifact file to deploy'}),
    // flag with no value (-f, --force)
    force: Flags.boolean({char: 'f'}),
  }

  static args = [{name: 'file'}]

  public async run(): Promise<void> {
    const {args, flags} = await this.parse(Deploy)

    console.log('Deploying to AWS ...')

    const fileName = flags.file ?? 'unknown'

    const user = new User('Jonh', 22)

    console.log(`Done. Deployed ${fileName} file to AWS by ${user.name}`)
    if (args.file && flags.force) {
      this.log(`you input --force and --file: ${args.file}`)
    }
  }
}
