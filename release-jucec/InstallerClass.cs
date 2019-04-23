using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace TCIReleaseJucec
{
    [RunInstaller(true)]
    public partial class InstallerClass : Installer
    {
        public InstallerClass()
        {
            InitializeComponent();
        }
    }
}