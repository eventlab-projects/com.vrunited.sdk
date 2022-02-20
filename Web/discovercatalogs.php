<?php

$platform = $_POST["BuildPlatform"];
$catalogs = array();
DiscoverCatalogs(".", $catalogs, $platform);

echo implode(";", $catalogs);

///////////////////////////////////////////

function DiscoverCatalogs($dir, &$result, $platform) 
{
    $scan = scandir($dir);
    foreach($scan as $file) 
    {
        $path = "$dir/$file";
        if (!is_dir($path)) 
        {
            $path_parts = pathinfo($path);
            if ($path_parts["extension"] == "json" && strpos($path, $platform)) 
            {
                array_push($result, $path);    
            }
        }
        else if ($file != "." && $file != "..") 
        {
            DiscoverCatalogs($path, $result, $platform);            
        }
    }
}

?>