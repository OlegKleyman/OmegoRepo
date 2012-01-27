﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class ReleaseBuilder
    {
        public string Name { get; set; }
        private readonly Regex _regex;
        private readonly string[] _keywords;
        public ReleaseBuilder(string name)
        {
            _regex = new Regex(@"\.S\d{2}E\d{2}\.", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _keywords = new[] { ".720P.", ".1080P.", ".DVDRIP.", ".PAL.DVDR.", ".NTSC.DVDR.", ".XVID." };
            Name = name;
        }

        public Release Build()
        {
            var release = new Release(GetReleaseType());
            release.Name = GetReleaseName(release);
            return release;
        }

        private string GetReleaseName(Release release)
        {
            string name = Name;
            if (release.ReleaseType == ReleaseType.Tv)
            {
                name = GetTvName(name);
            }
            return name;
        }

        private string GetTvName(string name)
        {
            var nameSplit = _regex.Split(name);
            name = nameSplit[0].Replace('.', ' ');
            return name;
        }

        private ReleaseType GetReleaseType()
        {
            var type = default(ReleaseType);
            
            if (_regex.IsMatch(Name))
            {
                type = ReleaseType.Tv;
            }
            else if (_keywords.Any(keyword => Name.ToUpper(CultureInfo.InvariantCulture).Contains(keyword)))
            {
                type = ReleaseType.Movie;
            }

            return type;
        }
    }
}