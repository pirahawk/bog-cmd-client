using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bog.Cmd.Common.Glob
{
    public static class FileGlobSearchUtility
    {
        public static string[] FindAllFiles(string searchDirectoryPath, params string[] mediaGlobPatterns)
        {
            if (mediaGlobPatterns == null) throw new ArgumentNullException(nameof(mediaGlobPatterns));

            if (string.IsNullOrWhiteSpace(searchDirectoryPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(searchDirectoryPath));

            if (!Directory.Exists(searchDirectoryPath) || mediaGlobPatterns.Length <= 0)
            {
                return Enumerable.Empty<string>().ToArray();
            }

            var allGlobs = mediaGlobPatterns.Select(pattern => DotNet.Globbing.Glob.Parse(pattern)).ToArray();
            var allFiles = Directory.GetFiles(searchDirectoryPath);
            var allResults = new ConcurrentBag<string>();
            var allFilesCountDownEvent = new CountdownEvent(allFiles.Length);
            
            Parallel.ForEach(allFiles,  (filePath, outerLoopState, fileIndex) =>
            {
                var fileName = Path.GetFileName(filePath);
                var globCountDown = new CountdownEvent(allGlobs.Length);

                Parallel.ForEach(allGlobs, (glob, innerLoopState, globIndex) =>
                {
                    if (glob.IsMatch(fileName))
                    {
                        allResults.Add(filePath);
                        
                        //THIS IS A BAD IDEA - MAKES THE RUN INDETERMINISTIC, JUST LET IT TEST EACH GLOB - COULD GET DUPLICATES BUT SO WHAT?
                        //try
                        //{
                        //    globCountDown.Signal(allGlobs.Length - globCountDown.CurrentCount);
                        //    Console.WriteLine($"glob#{globIndex}: upped countdown length. Killing loop now");
                        //    innerLoopState.Break();
                        //}
                        //catch (Exception e)
                        //{
                        //    // empty by design if I could not end the loop quickly,
                        //    // just let the other iterations run in this case
                        //}
                    }

                    try
                    {
                        globCountDown.Signal();
                    }
                    catch (Exception e)
                    {
                        // empty by design incase someone tried to end things early,
                        // the other threads will error if also trying to innocently decrement
                    }
                });

                globCountDown.Wait(-1);
                allFilesCountDownEvent.Signal();
            });

            allFilesCountDownEvent.Wait(-1);
            return allResults.ToArray();
        }
    }
}