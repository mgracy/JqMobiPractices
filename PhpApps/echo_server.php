<?php
	define("TOKEN", "weixin");

	$signature = $_GET['signature'];
	$nonce = $_GET['nonce'];
	$timestamp = $_GET['timestamp'];
	#echostr = $_GET['echostr'];

	$tmpArr = array($nonce, $timestamp, TOKEN);
	sort($tmpArr);

	$tmpStr = implode($tmpArr);

	$tmpStr = sha1($tmpStr);
	if($tmpStr == $signature) {
		echo $echostr;
	}
?>