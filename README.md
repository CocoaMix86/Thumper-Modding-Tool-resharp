# Thumper Modding Tool resharp

This is a C# recode of Rainbow's modding tool: https://github.com/RainbowUnicorn7297/thumper-modding-tool

# Key Differences
### JSON Formatting
The new modding tool follows correct JSON formatting for the custom level files. This affects the following from the old modding tool:  
1. Do not begin and end files with SQUARE BRACKETS. JSON starts and ends with a single pair of curly brackets.  

2. All strings must be wrapped in quotes. This mainly affects True/False statements  
old: `'step': False,`  
new: `'step': 'False'`  
Numbers don't need to be wrapped in quotes, such as `'beat_cnt': 64`

3. Curved brackets must now be square brackets.  
old: `'footer': (1,1,2,1,2,'kIntensityScale','kIntensityScale',1,1,1,1,1,1,1,1,0,0,0)`  
new: `'footer': [1,1,2,1,2,'kIntensityScale','kIntensityScale',1,1,1,1,1,1,1,1,0,0,0]`  
old: `'rails_color': (0.3, 1, 0, 1),`  
new: `'rails_color': [0.3, 1, 0, 1],`

4. Items inside `samp_` and `spn_` files must be inside a list ( JSON denotes lists by square brackets [] )  
a. To do this, put `'items': [` before the first item in these files, then close the square bracket at the end  
b. Then wrap the whole thing in curly brackets  
old: https://pastebin.com/qaCGdhef  
new: https://pastebin.com/wvDKkgTU

If you're comfortable running Powershell, you can open a console inside your custom levels folder and run this command:  
`Get-ChildItem | % {(Get-Content $_) -notmatch '^\s*$' -replace "^\[","" -replace "\]$","" -notmatch '^\s*$'  -replace "True","'True'" -replace "False","'False'" -replace "\(","[" -replace "\)","]" | Set-Content $_}`  
This will replace the brackets and surround True/False with quotes, and remove the leading and trailing square brackets.  
**IT DOES NOT** however do STEP 4 above, for `samp_` and `spn_` files.

### LEVEL DETAILS  
Custom levels must include a "LEVEL DETAILS.txt" file. This file contains a few helpful bits of information, like difficulty and the author's name.  
You can find a template for that here: https://github.com/CocoaMix86/Thumper-Modding-Tool-resharp/blob/master/templates/LEVEL%20DETAILS.txt
