# AAT_TextExtractor
**Ace Attorney Trilogy - Text Extractor** (or you can just call it "Text Script Simplifier")

**This tool will extract parameters from all Text(); and SetSpeakerId(); in AAT original script file to simplify them for translation\localization. 
This tool also supports inserting changes made to the extracted file back to the original script file as well.**

## Disclaimer:
I'm quite a newbie at writing application, so if my code is bad, please help me fix it. I appreciate every commits/forks for this project.

## Usage:
**Prequisite:**

Make sure that the original script files are already converted to .txt or else this wouldn't work. (might support auto convertion for later updates)

**Getting Started:**
1. Download the latest pre-build release and extracted the file to your working directory.

2. You can use the pre-defined pipeline folders or create your own.

3. Extraction process (from version v1.1.0 and further) will need you to manually add the SpeakerIDs and Speaker's names in SpeakerID.txt found in this tool's directory.
(There are some IDs and names provided as an example in the file, please follow the same formatting.)

4. Run the program (AAT_TextExtractor.exe), select file/process mode and enter required directories. (Read more about command at *Command Description* below.)

5. Wait for the extraction/insertion process to complete.

6. Finished! Yay!


## Command Description:

1. **Single File Mode:**
 
   - Single file mode let you select just one file for the operation, not using single file mode (simply called "multiple files mode") will let the program select every files in the specified folder. However, using multiple files mode works with just one single file as well.

2. **Process Mode:**
   - Extract: Extract parameters from all lines that started with "Text("" then write them in new text file. The lines' index will also be exported in .meta file for further insertion process.
   - Insert: Insert edited lines from extracted text relative to the original text by using meta file as a reference, then exported to a new text file.

3. **Input Directory:**
   - For Extraction: Input is the original text file/folder.
   - For Insertion: Input is the edited text file/folder.

4. **Original Directory:**
   - For Extraction: None. (referred as Input instead of Original.)
   - For Insertion: Original is the original text file/folder.

5. **Output Directory:**
   - For Extraction: Output is the folder that will store the extracted text and meta files.
   - For Insertion: Output is the folder that will store the inserted files.

## Compatibility Note:
From v1.1.0 and further, this tool will support backward compatibility in which it will add version header to every extracted text files. 
The insertion process will refer to the version header to select the correct insertion method.

If you updated the tool from 1.0.0 version you can add {AAT_TE version: 1.0.0} to the top-most line of your extracted file(s) to make it works with newer versions.

## FAQ:
1. **Why is there no output from extraction process?**
>**Ans:** Make sure that:
>- All files in the folder is .txt, if not, convert them.
>- Your input and output folders are correct.

2. **Why is extracted output empty?**
>**Ans:** Make sure that you extracted from the original file (the one with a lot of command lines and contains at least one Text(); method).

3. **Why insertion process tells me meta file doesn't exist?**
>**Ans:** Make sure that your meta files has the same name with the text files and are in the same folder as the text files. Also, make sure that your input directory is correct.

4. **Why insertion process tells me original file doesn't exist?**
>**Ans:** Make sure that your original files has the same name with the text files. Also, make sure that your original directory is correct.

5. **Why are lines in inserted file different from the original file.**
>**Ans:** Make sure that: 
>- Original files are unchanged throughout the process.
>- You haven't deleted/added any line from/to the extracted files.
>- You haven't changed the data inside meta files.
>- You haven't renamed your meta/text files.

6. **Why extraction process tells me that SpeakerID file is not found?**
>**Ans:** Make sure that you haven't removed SpeakerID.txt that comes with pre-build version. Also make sure that the SpeakerID file is in the same folder as the tool.

7. **Why insertion process tells me that "Version header is not found"**
>**Ans:** Make sure that in your edited file, there is {AAT_TE version: *(version)*} on the top-most line; if not, add it manually and replace *(version)* with current version. (Ex. {AAT_TE version: 1.1.0})
>
>**If you updated the tool from 1.0.0 version you can add {AAT_TE version: 1.0.0} to the top-most line to make it compatible with newer versions.**

8. **Why my extracted file has [Error: Missing Speaker Difinition]?**
>**Ans:** Make sure that your original files are unchanged and you have already added SpeakerID and Name in SpeakerID.text

9. **How can I know SpeakerID and Name?"**
>**Ans:** The only way to know is to look into the original script and find SetSpeakerId method and dialogues below it, then referrence to the playthrough to see who is speaking those dialogues (this shouldn't be difficult for AA fans). Still, I will continue to update [SpeakerIDs and Names in the Wiki][Wiki SpeakerIDs and Names], check my repo often for that matter.

[Wiki SpeakerIDs and Names]: https://github.com/MaFIaTH/AAT_TextExtractor/wiki#speakerids-and-names
