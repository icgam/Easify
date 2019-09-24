// This software is part of the EasyApi framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyApi.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EasyApi.Logging
{
    public sealed class ArgumentsFormatter : IArgumentsFormatter
    {
        private readonly ILogger<ArgumentsFormatter> _log;
        private readonly JsonSerializerSettings _serializerSettings;

        public ArgumentsFormatter(ILogger<ArgumentsFormatter> log, ArgumentFormatterOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = options.Formatting
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public string FormatArguments(object[] arguments)
        {
            return FormatArguments(arguments.ToList());
        }

        public string FormatArguments(List<object> arguments)
        {
            var sb = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument == null)
                    sb.Append(" argument is NULL,");
                else if (LastArgument(arguments, argument))
                    sb.Append($"{FormatArgument(argument)}.");
                else
                    sb.Append($"{FormatArgument(argument)}, ");
            }
            return sb.ToString();
        }

        private bool LastArgument(List<object> arguments, object argument)
        {
            return arguments.IndexOf(argument) == arguments.Count - 1;
        }

        public string FormatArgument(object argument)
        {
            try
            {
                if (argument == null)
                    return "[NULL]";

                if (argument.IsReferenceType() && argument.IsString() == false)
                {
                    return JsonConvert.SerializeObject(argument, _serializerSettings);
                }
                return argument.ToString();
            }
            catch (Exception ex)
            {
                var argumentType = argument != null ? argument.GetType().Name : "Arg is NULL, can't check type";

                _log.LogTrace(ex, $"Failed to format ARGUMENT/RETURN (type: '{argumentType}') value!");
                return "FAILED TO FORMAT ARGUMENT/RETURN VALUE";
            }
        }
    }
}