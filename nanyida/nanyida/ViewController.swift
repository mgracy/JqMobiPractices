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
    
    @IBAction func btnLogin(sender: UIButton) {
        //表单提交前的验证
        if stuNo.text == "" || stuPwd.text == "" || stuNo.text == "请输入学号" || stuPwd.text == "请输入密码"
        {
            var alert = UIAlertController(title: "Tips", message: "username or password is required!", preferredStyle: UIAlertControllerStyle.Alert)
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

