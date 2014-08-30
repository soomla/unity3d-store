#!/usr/bin/python

from mod_pbxproj import *
from os import path, listdir
from shutil import copytree
import sys

frameworks = [
  'System/Library/Frameworks/Security.framework',
  'usr/lib/libsqlite3.0.dylib',
  'System/Library/Frameworks/StoreKit.framework'
]

weak_frameworks = [

]

pbx_file_path = sys.argv[1] + '/Unity-iPhone.xcodeproj/project.pbxproj'
pbx_object = XcodeProject.Load(pbx_file_path)

for framework in frameworks:
  pbx_object.add_file(framework, tree='SDKROOT')

for framework in weak_frameworks:
  pbx_object.add_file(framework, tree='SDKROOT', weak=True)

pbx_object.add_other_ldflags('-ObjC')

pbx_object.save()
