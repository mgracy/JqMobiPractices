//
//  ViewController.swift
//  nanyida
//
//  Created by  马桂新 on 14/11/24.
//  Copyright (c) 2014年 mgracy.blog.163.com. All rights reserved.
//

import UIKit
//import "ASIHTTPRequest.h"

class ViewController: UIViewController {

    @IBOutlet var stuNo: UITextField!
    
    @IBOutlet var stuPwd: UITextField!
    
    @IBAction func btnReset(sender: UIButton) {
        stuNo.text = "113200700100079"
        stuPwd.text = "113200700100079"
    }
    @IBAction func btnLogin(sender: UIButton) {
        //表单提交前的验证
        if stuNo.text == "" || stuPwd.text == "" || stuNo.text == "请输入学号" || stuPwd.text == "请输入密码"
        {
            var alert = UIAlertController(title: "Tips", message: "username or password is required!", preferredStyle: UIAlertControllerStyle.Alert)
            var actionCancel = UIAlertAction(title: "Cancel", style: UIAlertActionStyle.Default, handler: nil)
            alert.addAction(actionCancel)
        self.presentViewController( alert, animated: true, completion: nil)
        }
        //隐藏键盘
        stuNo.resignFirstResponder()
        stuPwd.resignFirstResponder()
        sendRequest()
        /*
        NSURL url = NSURL(string: "http://www.baidu.com")
        ASIHTTPRequest *request =
        ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL:url];
        [request startSynchronous];
        NSError *error = [request error];
        if (!error) {
            NSString *response = [request responseString];
        }
        */
       // http://donwait.vicp.cc:100/smu/student.ashx?A=signon&U=113200700100079&P=113200700100079

        let urlString: String? = "http://donwait.vicp.cc:100/smu/student.ashx?A=signon&U=\(stuNo.text)&P=\(stuPwd.text)"
        var url:NSURL?
        var requrst:NSURLRequest?
        var conn:NSURLConnection?
        url = NSURL.URLWithString(urlString!)
        requrst = NSURLRequest(URL:url!)
        conn = NSURLConnection(request: requrst!,delegate: self)
    
        /*
        if(conn){
            println("http连接成功!")
        }else{
            println("http连接失败!")
        }
        */
    }
    func connection(connection:NSURLConnection!,didReceiveData data:NSData!){
        var returnString:NSString?
        returnString = NSString(data:data,encoding:NSUTF8StringEncoding)
        println(returnString)
      
        if returnString!.substringWithRange(NSMakeRange(10,1)) == "0"
        {
        var alert = UIAlertController(title: "提示", message: "登录失败", preferredStyle: UIAlertControllerStyle.Alert)
        var actionCancel = UIAlertAction(title: "Cancel", style: UIAlertActionStyle.Default, handler: nil)
        alert.addAction(actionCancel)
        self.presentViewController( alert, animated: true, completion: nil)
            return
        }
        else
        {
       /* var alert = UIAlertController(title: "提示", message: "登录成功", preferredStyle: UIAlertControllerStyle.Alert)
        var actionCancel = UIAlertAction(title: "Cancel", style: UIAlertActionStyle.Default, handler: nil)
        alert.addAction(actionCancel)
        self.presentViewController( alert, animated: true, completion: nil)
*/
            var mainStoryboard = UIStoryboard(name: "Main", bundle: nil)
         var indexVC =   mainStoryboard.instantiateViewControllerWithIdentifier("indexViewController") as ViewController
            var tabbarController = mainStoryboard.instantiateViewControllerWithIdentifier("tabbarController") as UITabBarController
            tabbarController.tabBarItem.image = UIImage(named: "tabbar_bg.png")
            tabbarController.navigationController?.setNavigationBarHidden(true, animated: true)
            self.presentViewController(tabbarController, animated: true, completion: nil)
        self.presentViewController(indexVC, animated: true, completion: nil)
        }
    }

    func sendRequest() {
    if let text = self.stuNo.text {
        let url = NSURL(string:text)
        let request = NSURLRequest(URL:url)
        //self.webView.loadRequest(request)
    }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }


}

