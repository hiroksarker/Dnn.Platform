﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


using DotNetNuke.Entities.Users;
using DotNetNuke.Tests.Utilities;

namespace DotNetNuke.Tests.Urls
{
    public class TestUtil
    {
        internal static void AddUser(int portalId, string userName, string password, string vanityUrl)
        {
            var user = UserController.GetUserByName(portalId, userName);
            if (user == null)
            {
                //Add User
                user = new UserInfo
                {
                    PortalID = portalId,
                    Username = userName,
                    Email = userName + "@change.me",
                    VanityUrl = vanityUrl,
                    Membership = { Password = password, Approved = true }
                };
                UserController.CreateUser(ref user);
            }
            else
            {
                //Update User
                user.VanityUrl = vanityUrl;
                user.IsDeleted = false;
                UserController.UpdateUser(portalId, user);
            }
        }

        internal static void DeleteUser(int portalId, string userName)
        {
            var user = UserController.GetUserByName(portalId, userName);
            if (user != null)
            {
                UserController.RemoveUser(user);
            }
        }


        internal static string EmbeddedFilePath
        {
            get { return "DotNetNuke.Tests.Urls.TestFiles"; }
        }

        internal static string FilePath
        {
            get
            {
                var uri = new System.Uri(Assembly.GetExecutingAssembly().CodeBase);
                string path = Path.GetFullPath(uri.AbsolutePath).Replace("%20", " ");

                return Path.Combine(path.Substring(0, path.IndexOf("bin", System.StringComparison.Ordinal)), "TestFiles");
            }
        }

        internal static string GetEmbeddedFileName(string fileName)
        {
            string fullName = String.Format("{0}.{1}", EmbeddedFilePath, fileName);
            if (!fullName.ToLowerInvariant().EndsWith(".csv"))
            {
                fullName += ".csv";
            }

            return fullName;
        }

        internal static Stream GetEmbeddedFileStream(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbeddedFileName(fileName));
        }

        internal static void GetReplaceCharDictionary(Dictionary<string, string> testFields, Dictionary<string, string> replaceCharacterDictionary)
        {
            string replaceCharWithChar = testFields.GetValue("ReplaceChars");

            if (!String.IsNullOrEmpty(replaceCharWithChar))
            {
                string[] pairs = replaceCharWithChar.Split(';');
                foreach (string pair in pairs)
                {
                    string[] vals = pair.Split(':');
                    string key = null;
                    string val = null;
                    if (vals.GetUpperBound(0) >= 0)
                    {
                        key = vals[0];
                    }
                    if (vals.GetUpperBound(0) >= 1)
                    {
                        val = vals[1];
                    }
                    if (key != null && val != null)
                    {
                        replaceCharacterDictionary.Add(key, val);
                    }
                }
            }
        }

        internal static string ReadStream(string fileName)
        {
            return Util.ReadStream(FilePath, fileName);
        }

        internal static void ReadStream(string fileName, Action<string, string> onReadLine)
        {
            Util.ReadStream(FilePath, fileName, onReadLine);
        }
    }
}
