cls
function Remove-Update {
#卸载补丁的安装天数配置
$TimeOutDays=7
[int]$count = 0;
$hotfixes=(Get-HotFix | Sort-Object -Property installedon -Descending)
#$lis= (Get-HotFix | Sort-Object -Property installedon -Descending)
 foreach ($hotfix in $hotfixes) 
        {  $count = $count + 1              
           $installtime=$hotfix | Select-Object -ExpandProperty installedon           
           $daypan=((get-date)-$installtime).days 
          # $lis= Get-HotFix | Sort-Object -Property installedon -Descending
           if ($TimeOutDays -gt $daypan ) 
              {  "Inside first if" 
                $KBID = $hotfix.HotfixId.Replace("KB", "") 
                write-host $kbid
                $RemovalCommand = "wusa.exe /uninstall /kb:$KBID /quiet /norestart" 
                Write-Host "卸载 KB$KBID ，安装时间："$installtime
                Invoke-Expression $RemovalCommand          
    while (@(Get-Process wusa -ErrorAction SilentlyContinue).Count -ne 0)         
            { Start-Sleep 3 
              Write-Host "wusa.exe正在运行 ...卸载中..." }
      }  }
}
Remove-Update