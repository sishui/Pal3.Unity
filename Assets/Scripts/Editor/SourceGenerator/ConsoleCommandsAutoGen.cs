// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2024, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Editor.SourceGenerator
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Base;
    using IngameDebugConsole;
    using Pal3.Core.Command;
    using Pal3.Core.Command.SceCommands;
    using Pal3.Game.Command;
    using UnityEngine;

    /// <summary>
    /// ConsoleCommands.cs source generator
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public class ConsoleCommandsAutoGen<TCommand> : ISourceGenerator
    {
        public void GenerateSourceClass(CodeWriter writer, string className, string nameSpace)
        {
            // Header.
            writer.WriteLine("// ---------------------------------------------------------------------------------------------");
            writer.WriteLine("// <auto-generated>");
            writer.WriteLine($"//     This code was auto-generated by {GetType().Name}");
            writer.WriteLine("// </auto-generated>");
            writer.WriteLine("// ---------------------------------------------------------------------------------------------");
            writer.WriteLine();

            // Begin namespace.
            writer.WriteLine($"namespace {nameSpace}");
            writer.BeginBlock();

            // Begin using.
            writer.WriteLine("using Core.Command;");
            writer.WriteLine("using Core.Command.SceCommands;");
            writer.WriteLine("using Extensions;");
            writer.WriteLine("using IngameDebugConsole;");
            writer.WriteLine();

            // Begin class.
            writer.WriteLine($"// <auto-generated/>");
            writer.WriteLine($"public static class {className}");
            writer.BeginBlock();

            Type[] commands =  Utility.GetTypesOfInterface(typeof(TCommand))
                .Where(IsCommandAvailableInConsole).ToArray();

            Debug.Log($"[{nameof(ConsoleCommandsAutoGen<TCommand>)}] Found {commands.Length} available console commands");

            for (var i = 0; i < commands.Length; i++)
            {
                string commandName = commands[i].Name.Replace("Command", string.Empty);
                PropertyInfo[] properties = commands[i].GetProperties();

                writer.WriteLine($"// <auto-generated/>");
                string attributeName = nameof(ConsoleMethodAttribute);
                writer.WriteLine($"[{attributeName}(\"{commandName}\", \"{GetCommandDescription(commands[i])}\")]");
                writer.WriteLine($"public static void {commandName}" +
                       $"({Utility.GetMethodArgumentDefinitionListAsString(properties)})");

                writer.BeginBlock();
                writer.WriteLine($"Pal3.Instance.Execute(new {commands[i].Name}" +
                                 $"({Utility.GetMethodArgumentListAsString(properties)}));");
                writer.EndBlock();

                if (i < commands.Length - 1)
                {
                    writer.WriteLine();
                }
            }

            // End class.
            writer.EndBlock();

            // End namespace.
            writer.EndBlock();
        }

        private string GetCommandDescription(Type command)
        {
            if (command.GetCustomAttribute<SceCommandAttribute>() is { } attribute)
            {
                return attribute.Description;
            }

            return $"Execute {command.Name}.";
        }

        private static bool IsCommandAvailableInConsole(Type command)
        {
            // object[] is not supported by InGameDebugConsole.
            if (command == typeof(DialogueAddSelectionsCommand)) return false;

            // Make all SceCommands available in console + all commands with AvailableInConsoleAttribute.
            return command.GetCustomAttributes()
                .Any(attribute => attribute is AvailableInConsoleAttribute or SceCommandAttribute);
        }
    }
}