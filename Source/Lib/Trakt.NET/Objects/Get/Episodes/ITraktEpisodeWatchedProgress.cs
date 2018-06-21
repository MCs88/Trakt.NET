﻿namespace TraktNet.Objects.Get.Episodes
{
    using System;

    public interface ITraktEpisodeWatchedProgress : ITraktEpisodeProgress
    {
        DateTime? LastWatchedAt { get; set; }
    }
}
