package com.pushspring.cli;

import com.pushspring.cli.commands.DeployCommand;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.ExitCodeGenerator;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import picocli.CommandLine;
import picocli.CommandLine.*;

@SpringBootApplication
public class CliApplication implements CommandLineRunner, ExitCodeGenerator {
    private IFactory factory;
    private DeployCommand deployCommand;
    private int exitCode;

    // constructor injection
    CliApplication(IFactory factory, DeployCommand deployCommand) {
        this.factory = factory;
        this.deployCommand = deployCommand;
    }

    @Override
    public void run(String... args) {
        // let picocli parse command line args and run the business logic
        exitCode = new CommandLine(deployCommand, factory).execute(args);
    }

    @Override
    public int getExitCode() {
        return exitCode;
    }

    public static void main(String[] args) {
        // let Spring instantiate and inject dependencies
        System.exit(SpringApplication.exit(SpringApplication.run(CliApplication.class, args)));
    }

}
