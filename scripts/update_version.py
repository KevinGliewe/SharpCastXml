#!/usr/bin/env python3
import fileinput
import os
import sys

script_dir = os.path.dirname(os.path.realpath(__file__))
repo_dir = os.path.abspath(os.path.join(script_dir, '..'))
props_file = os.path.join(repo_dir, 'Version.props')

version = '0.0.0'

if len(sys.argv) > 1:
    version = sys.argv[1]

find = "<VersionPrefix>0.0.0</VersionPrefix>"
replace = f"<VersionPrefix>{version}</VersionPrefix>"

print(f"Updating version in {props_file}")
print(f"  from {find} to {replace}")



with fileinput.FileInput(props_file, inplace=True, backup='.bak') as file:
    for line in file:
        print(line.replace(find, replace), end='')