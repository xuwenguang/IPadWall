import sys
import os
import shutil

targetFileName="/Users/nick_zhao/Downloads/wetransfer-70de2b/A1_new_2500k.mp4"
newFolderPath="/Users/nick_zhao/Desktop/200Man"
if not os.path.exists(newFolderPath):
	os.makedirs(newFolderPath)	


name="copyTest"
extension=".mp4"
for i in range(1,201):
	fullname=name+str(i)+extension
	NewName=os.path.join(newFolderPath,fullname)
	shutil.copy(targetFileName,NewName)
	os.rename
	pass



		
		