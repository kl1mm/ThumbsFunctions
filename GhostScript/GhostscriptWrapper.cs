using System.Collections;

namespace kli.ThumbsFunctions.GhostScript
{
    internal static class GhostscriptWrapper
    {

        public static void GenerateThumbnail(string inputPath, string outputPath)
        {
            GhostScript32.CallAPI(CreateArgs(inputPath, outputPath));
        }

        private static string[] CreateArgs(string inputPath, string outputPath)
        {
	        var args = new ArrayList(DefaultArgs)
	        {
		        "-sDEVICE=jpeg",
		        "-dJPEGQ=90",
		        "-dFirstPage=1", 
		        "-dLastPage=1",
		        $"-dDEVICEXRESOLUTION={96}", 
		        $"-dDEVICEYRESOLUTION={96}", 
		        $"-sOutputFile={outputPath}", 
		        inputPath
	        };

	        return (string[])args.ToArray(typeof(string));
        }

        private static readonly string[] DefaultArgs = {

                "-q",                       // Keep gs from writing information to standard output
                "-dQUIET",
                "-dPARANOIDSAFER",          // Run this command in safe mode
                "-dBATCH",                  // Keep gs from going into interactive mode
                "-dNOPAUSE",                // Do not prompt and pause for each page
                "-dNOPROMPT",               // Disable prompts for user interaction           
                "-dMaxBitmap=500000000",    // Set high for better performance
				"-dNumRenderingThreads=4",  // Multi-core, come-on!
                "-dAlignToPixels=0",
                "-dGridFitTT=0",
                "-dTextAlphaBits=4",
                "-dGraphicsAlphaBits=4"
        };
    }
}
