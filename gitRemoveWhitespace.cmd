@echo off

rem ignore whitespace inline (-b) or global (-w)

git diff -b > ../gitdiff
git reset --hard
git apply --ignore-space-change ../gitdiff

@pause



