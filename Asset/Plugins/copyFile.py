import sys
import os
import shutil

targetFileName="C:\Users\wxu\Desktop\copyTest.mp4"
newFolderPath="C:\Users\wxu\Desktop\NewImageFolder"
if not os.path.exists(newFolderPath):
	os.makedirs(newFolderPath)	


name="copyTest"
extension=".mp4"
for i in range(1,301):
	fullname=name+str(i)+extension
	NewName=os.path.join(newFolderPath,fullname)
	shutil.copy(targetFileName,NewName)
	os.rename
	pass



		
		