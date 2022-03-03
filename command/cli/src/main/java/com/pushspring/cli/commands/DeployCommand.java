package com.pushspring.cli.commands;

import org.springframework.stereotype.Component;
import picocli.CommandLine.*;

import java.util.concurrent.Callable;

@Component
@Command(name = "deployCommand")
public class DeployCommand implements Callable<Integer> {

    @Option(names = "--file", description = "Jar file")
    String file;

    @Override
    public Integer call() throws Exception {
        System.out.println("Deployed, file: " + file);
        return 0;
    }
}
