# create new branch and checkout
git checkout -b new_branch

# list all merged in "master" branches
git checkout master
git branch --merged

# remove local branch
git branch -d old_merged_branch

# list all remote branch references
git branch -r

# clean up outdated remote branch references
git remote prune origin