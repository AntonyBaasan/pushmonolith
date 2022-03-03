package com.pushspring.cli.commands;

import com.pushspring.cli.services.interfaces.IDeploymentService;
import org.springframework.stereotype.Component;
import picocli.CommandLine.*;

import java.util.concurrent.Callable;

@Component
@Command(name = "deployCommand")
public class DeployCommand implements Callable<Integer> {

    private final IDeploymentService deploymentService;

    public DeployCommand(IDeploymentService deploymentService) {
        this.deploymentService = deploymentService;
    }

    @Option(names = "--file", description = "Jar file")
    String file;

    @Override
    public Integer call() throws Exception {
        deploymentService.setFile(file);
        deploymentService.exec();
        return 0;
    }
}
