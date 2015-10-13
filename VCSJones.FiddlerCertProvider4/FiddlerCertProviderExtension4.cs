﻿using System;

namespace VCSJones.FiddlerCertProvider4
{
    public class FiddlerCertProviderExtension4 : Fiddler.IFiddlerExtension
    {

        public FiddlerCertProviderExtension4()
        {
            var codeBase = new Uri(typeof(FiddlerCertificate).Assembly.CodeBase);
            if (codeBase.Scheme != Uri.UriSchemeFile)
            {
                throw new ArgumentException();
            }
            var path = codeBase.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            var normalized = System.IO.Path.GetFullPath(path);
            Fiddler.FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.assembly", normalized);
        }

        public void OnBeforeUnload()
        {
        }

        public void OnLoad()
        {
        }
    }
}