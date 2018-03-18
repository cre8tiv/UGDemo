# UGDemo
Users Group Demo

The goal of this demo is to illustrate how to enable the rapid provisioning of developer tools, environments and applications using BoxStarter, Chocolately, Vagrant and Docker.

To run this project:

1. Set execution policy to unrestricted in PowerShell: Set-ExecutionPolicy Unrestricted
2. From admin Powershell prompt: . { iwr -useb http://boxstarter.org/bootstrapper.ps1 } | iex; get-boxstarter -Force
3. From admin Powershell prompt: refreshenv
4. From admin Powershell prompt: Install-BoxstarterPackage -PackageName https://gist.githubusercontent.com/cre8tiv/b78c10010909f5e95b1240c3f504c2bc/raw/40d79bb84e67402617b8029c17a59283db7f1874/BoxstarterWorkstation.txt -DisableReboots
5. Download repository from Git
6. In the repository directory, vagrant up


