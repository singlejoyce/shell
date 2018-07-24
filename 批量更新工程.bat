@echo off
for /d %%v in (*) do (
svn cleanup %%v 
svn revert --depth=infinity %%v   
svn update %%v   
)

pause