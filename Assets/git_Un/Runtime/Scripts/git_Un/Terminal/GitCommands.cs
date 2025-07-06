using System;

public static class GitCommands
{
    //TODO add SPACE after command
    public static string log_oneline = "git log --oneline ";
    public static string echo_hello = "echo hello";
    public static string merge_base= "git merge-base ";
    public static string rev_parse = "git rev-parse ";
    public static string g_switch = "git switch ";
    public static string merge = "git merge ";
    public static string merge_Xours = "git merge -Xours ";
    public static string add_all = "git add . ";
    public static string add= "git add ";
    public static string commit_m = "git commit -m ";
    public static string diff = "git diff ";
    public static string subtree_split_prefix = "git subtree split --prefix="; //no space intentional
    public static string push_origin = "git push origin ";
    public static string push_u_origin = "git push -u origin ";
    public static string branch = "git branch ";
    public static string init = "git init ";
    public static string fetch = "git fetch ";
    public static string add_remote = "git remote add origin ";
    public static string cat_file_p = "git cat-file -p ";
    public static string checkout = "git checkout ";
    public static string check_merged = "git branch --merged ";
    public static string pull = "git pull ";



}

public static class GitFlags
{
    //TODO add SPACE before and after flag
    public static string no_edit = " --no-edit ";
    public static string branch= " --branch ";
    public static string all = " --all ";
    public static string show_current = " --show-current ";

}

public static class BashCommands
{
    public static string touch = "touch ";
    public static string mkdir = "mkdir ";
    public static string curl_o= "curl -o ";
    public static string echo= "echo  ";
    public static string echo_e = "echo -e ";

}