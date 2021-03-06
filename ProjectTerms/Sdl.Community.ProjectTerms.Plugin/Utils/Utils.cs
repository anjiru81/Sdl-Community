﻿using Sdl.Community.ProjectTerms.Plugin.Exceptions;
using Sdl.ProjectAutomation.FileBased;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sdl.Community.ProjectTerms.Plugin.Utils
{
    public static class Utils
    {
        public static void RemoveDirectoryFiles(string directoryPath)
        {
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                File.Delete(file);
            }
        }

        public static void RemoveDirectory(string directoryPath)
        {
            try
            {
                RemoveDirectoryFiles(directoryPath);
                Directory.Delete(directoryPath);
            }
            catch (Exception e)
            {
                throw new ProjectTermsException(PluginResources.Error_RemoveDirectory + e.Message);
            }
        }

        public static void CreateDirectory(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                else
                {
                    RemoveDirectoryFiles(directoryPath);
                }
            }
            catch (Exception e)
            {

                throw new ProjectTermsException(PluginResources.Error_CreateDirectory + e.Message);
            }
        }

        public static bool VerifyRegexPattern(String pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch { }
            return false;
        }

        public static bool VerifySingleFileProjectType()
        {
            var project = SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject;
            FieldInfo projectVal = typeof(FileBasedProject).GetField(PluginResources.Constant_ProjectType, BindingFlags.NonPublic | BindingFlags.Instance);

            dynamic projectValDynamic = projectVal.GetValue(project);
            dynamic projectType = projectValDynamic.ProjectType != null ? projectValDynamic.ProjectType : string.Empty;

            string projectTypeContent = Convert.ToString(projectType);

            return projectTypeContent.Equals(PluginResources.Constant_ProjectTypeContent);
        }

        public static string GetXMLFilePath(string projectPath, bool wordCloudFile = false)
        {
            if (!wordCloudFile)
                return Path.Combine(projectPath + "\\tmp", Path.GetFileNameWithoutExtension(projectPath) + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".xml");
            else
                return Path.Combine(projectPath, PluginResources.WordCloudFileName + ".xml");
        }

        public static string GetProjecPath()
        {
            return SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject.GetProjectInfo().LocalProjectFolder;
        }

        public static FileBasedProject GetCurrentProject()
        {
            return SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject;
        }

        public static string GenerateBlackListPath()
        {
            return Path.Combine(Utils.GetProjecPath(), PluginResources.BlacklistFileName);
        }

        public static string GetExistedFileName(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            return files.Count() == 1 ? Path.GetFileName(files.FirstOrDefault()) : string.Empty;
        }

        public static string GetSelectedProjectPath()
        {
            return SdlTradosStudio.Application.GetController<ProjectsController>().SelectedProjects.First().FilePath;
        }
    }
}
