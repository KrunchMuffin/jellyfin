﻿using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using System.Collections.Generic;
using MediaBrowser.Model.Serialization;
using System;

namespace MediaBrowser.Controller.Entities
{
    public class MusicVideo : Video, IHasArtist, IHasMusicGenres, IHasLookupInfo<MusicVideoInfo>
    {
        [IgnoreDataMember]
        public string[] Artists { get; set; }

        public MusicVideo()
        {
            Artists = new string[] {};
        }

        [IgnoreDataMember]
        public string[] AllArtists
        {
            get
            {
                return Artists;
            }
        }

        public override UnratedItem GetBlockUnratedType()
        {
            return UnratedItem.Music;
        }

        public MusicVideoInfo GetLookupInfo()
        {
            var info = GetItemLookupInfo<MusicVideoInfo>();

            info.Artists = Artists;

            return info;
        }

        public override bool BeforeMetadataRefresh(bool replaceAllMetdata)
        {
            var hasChanges = base.BeforeMetadataRefresh(replaceAllMetdata);

            if (!ProductionYear.HasValue)
            {
                var info = LibraryManager.ParseName(Name);

                var yearInName = info.Year;

                if (yearInName.HasValue)
                {
                    ProductionYear = yearInName;
                    hasChanges = true;
                }
                else
                {
                    // Try to get the year from the folder name
                    if (!IsInMixedFolder)
                    {
                        info = LibraryManager.ParseName(System.IO.Path.GetFileName(ContainingFolderPath));

                        yearInName = info.Year;

                        if (yearInName.HasValue)
                        {
                            ProductionYear = yearInName;
                            hasChanges = true;
                        }
                    }
                }
            }

            return hasChanges;
        }
    }
}
