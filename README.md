# AAT_TextExtractor
**Ace Attorney Trilogy - Text Extractor (or you can just can it "Text Script Simplifier")**

**This tool with extract parameter from all Text(); in AAT original script file to simplify them for translation\localization. 
This tool also supports inserting changes made to the extracted file back to the original script file as well.**

## Disclaimer:
I'm quite a newbie at writing application, so if my code is bad then please help me fix it. I appreciate every commit/fork for this project.

## Usage:
**Prequisite:**
Make sure that the original script file is already converted to .txt or else this wouldn't work. (might support auto convertion for later updates)

**Getting Started:**
1. Download the latest pre-build release and extracted the file to your working directory.

2. You can use the pre-defined pipeline folders or create your own.

3. Run the program (AAT_TextExtractor.exe), select file/process mode and enter required directories. (Read more about command at *Command Description* below)

4. Wait for the extraction/insertion process to complete.

5. Finished! Yay!


## Command Description:

1. **Single File Mode:** 
Single file mode let you select just one file for the operation, not using single file mode will let the program select every files in the specified folder.

2. **Process Mode:**
2.1. Extract: Extract parameter from all lines that started with "Text(" then write them in new text file. The lines index will also be exported in .meta file for further insertion process.
2.2. Insert: Insert edited lines from extracted text relative to the original text by using meta file as a reference. Then exported to a new text file.

3. **Input Directory:**
3.1. For Extraction: Input is the original text file/folder
3.2. For Insertion: Input is the edited text file/folder

4. **Original Directory:**
4.1. For Extraction: None (referred as Input instead of Original)
4.2. For Insertion: Original is the original text file/folder

5. **Output Directory:**
5.1. For Extraction: Output is the folder that will store the extracted text and meta files
5.2. For Insertion: Output is the folder that will store the inserted files.

## FAQ:
1. **Why is there no output from extraction process?**
**Ans:** Make sure that:
- All files in the folder is .txt, if not, convert them.
- Your input and output folder are correct.

2. **Why is extracted output empty?**
**Ans:** Make sure that you're extracted from the original file (the one with a lot of command lines and contains at least one Text("text"); ).

3. **Why insertion process tells me meta file doesn't exist?**
**Ans:** Make sure that your meta files has the same name with the text file and is in the same folder as the text file. Also, make sure that your input directory is correct.

4. **Why insertion process tells me original file doesn't exist?**
Ans: Make sure that your original files has the same name with the text file. Also, make sure that your original directory is correct.

5. **Why are lines in inserted file different from the original file.**
**Ans:** Make sure that: 
- Original file is unchanged throughout the process.
- You haven't deleted any line from the extracted file.
- You haven't changed the data inside meta file.
- You haven't renamed your meta/text file.

