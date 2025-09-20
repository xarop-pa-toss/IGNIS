using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TesseractTrainer;

public class SplitTrainingText
{
    private string PROJECT_DIR = Path.Combine(AppContext.BaseDirectory, @"..\..\..\");
    private Random Rand = new Random();

    private string TRAINING_TEXT_FILE, OUTPUT_DIR, UNICHAR_FILE;
    private int RUN_CYCLES = 100;
    private string[] _lines;

    public SplitTrainingText()
    {
        UNICHAR_FILE = Path.GetFullPath(Path.Combine(PROJECT_DIR, "langdata\\eng.unicharset"));
        TRAINING_TEXT_FILE = Path.GetFullPath(Path.Combine(PROJECT_DIR, "langdata\\eng.training_text"));
        OUTPUT_DIR = Path.GetFullPath(Path.Combine(PROJECT_DIR,"tesstrain\\data\\Sinclair-ground-truth"));
        
        if (!Path.Exists(TRAINING_TEXT_FILE))
        {
            throw new FileNotFoundException("Training file or directory could not be found.");
        }
        if (!Path.Exists(OUTPUT_DIR))
        {
            Directory.CreateDirectory(OUTPUT_DIR);
        }
        
        _lines = ReadAllLinesFromFile(TRAINING_TEXT_FILE);
        Rand.Shuffle(_lines.ToArray());

        int lineCount = 0;
        string trainingTextFileName = Path.GetFileName(TRAINING_TEXT_FILE);

        foreach (string line in _lines)
        {
            string lineTrainingTextFile = Path.Combine(OUTPUT_DIR, $"{trainingTextFileName}_{lineCount}.gt.text");
            string lineTrainingTextFileWsl = lineTrainingTextFile.Replace(@"\", "/").Replace("C:", "/mnt/c");
            
            File.WriteAllText(lineTrainingTextFile, line);

            string fileBaseName = $"eng_{lineCount}";
            string wslUnicharFile = "/home/cepheus/langdata/eng.unicharset";
            string wslTrainingTextFile = "/home/cepheus/langdata/eng.training_text";
            string wslOutputDir = "/home/cepheus/tessout";
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "wsl.exe",
                Arguments = $"-c \"/home/cepheus/tesseract/text2image " +
                            $"--font='FS Sinclair Trial Medium' " +
                            $"--text={wslTrainingTextFile} " +
                            $"--outputbase={wslOutputDir}/{fileBaseName} " +
                            $"--max_pages=1 " +
                            $"--strip_unrenderable_words " +
                            $"--leading=32 " +
                            $"--xsize=3600 " +
                            $"--ysize=480 " +
                            $"--char_spacing=1.0 " +
                            $"--exposure=0 " +
                            $"--unicharset_file={wslUnicharFile}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
    
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine(e.Data); // prints stdout as it arrives
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"[ERR] {e.Data}"); // prints stderr as it arrives
                };
                
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
    
                process.WaitForExit();
            }

            lineCount++;
        }
    }

    private string[] ReadAllLinesFromFile(string path)
    {
        string[] lines = [];
        try
        {
            lines = File.ReadAllLines(TRAINING_TEXT_FILE);
        }
        catch (DirectoryNotFoundException e1)
        {
            Console.WriteLine("Specified directory does not exist.");
        }
        catch (FileNotFoundException e2)
        {
            Console.WriteLine("File not found in the given path: " + TRAINING_TEXT_FILE);
        }
        catch (Exception e3)
        {
            Console.WriteLine("Failed to read lines from file. Exception: " + e3.Message);
        }
        
        return lines;
    }
}