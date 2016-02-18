// Voidstone
// - Voidstone
// -- FFMPEG.cs
// -------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2016. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Jeffijoe.Voidstone
{
    /// <summary>
    ///     FFMPEG runner.
    /// </summary>
    public static class FFMPEG
    {
        #region Public Methods and Operators

        /// <summary>
        /// Starts encoding asynchronously.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task<bool> StartAsync(FFMPEGOptions options)
        {
            return Task.Run(() => Start(options));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts encoding.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool Start(FFMPEGOptions options)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var ffmpegPath = Path.Combine(baseDirectory, "ffmpeg.exe");
            var ffmpegArgs =
                string.Format(
                    "-y -f image2 -r {0} -i {1} -vcodec libx264 -profile:v high444 -crf 0 -preset veryfast \"{2}\"", 
                    options.Framerate, 
                    Path.Combine(options.InputDirectory, options.InputFilePattern), 
                    options.OutputFile);

            // Start a CMD in the background, which itself will run FFMPEG.
            var cmdArgs = string.Format("/C \"\"{0}\" {1}\"", ffmpegPath, ffmpegArgs);
            var startInfo = new ProcessStartInfo("cmd.exe", cmdArgs)
            {
                UseShellExecute = true, 
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode == 0;
        }

        #endregion
    }

    /// <summary>
    ///     The ffmpeg options.
    /// </summary>
    public class FFMPEGOptions
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the framerate.
        /// </summary>
        public int Framerate { get; set; }

        /// <summary>
        ///     Gets or sets the input directory.
        /// </summary>
        public string InputDirectory { get; set; }

        /// <summary>
        ///     Gets or sets the input file pattern.
        /// </summary>
        public string InputFilePattern { get; set; }

        /// <summary>
        ///     Gets or sets the output file.
        /// </summary>
        public string OutputFile { get; set; }

        #endregion
    }
}