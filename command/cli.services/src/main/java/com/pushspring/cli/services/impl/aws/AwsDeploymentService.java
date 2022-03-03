package com.pushspring.cli.services.impl.aws;

import com.pushspring.cli.services.interfaces.IDeploymentService;
import org.springframework.stereotype.Service;

@Service
public class AwsDeploymentService implements IDeploymentService {

    private String file;

    @Override
    public void setFile(String file) {
        this.file = file;
    }

    @Override
    public void exec() {
        if (this.file == null || this.file.equals("")) {
            System.out.println("Invalid file!");
        }

        System.out.println("Executed AWS deployment! file " + this.file + " was deployed on EC2");
    }
}
