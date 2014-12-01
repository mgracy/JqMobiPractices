//
//  ViewController.swift
//  myApp
//
//  Created by  马桂新 on 14/11/27.
//  Copyright (c) 2014年 mgracy.blog.163.com. All rights reserved.
//

import UIKit

class ViewController: UIViewController {

    @IBOutlet var txtInput: UITextField!
    @IBAction func btnGo(sender: UIButton) {
        if txtInput.text == "0"
        {
       // self.performSegueWithIdentifier("loginSeque", sender: self)
            var sb = UIStoryboard().instantiateViewControllerWithIdentifier("SecondStoryboard") as ViewController
            self.presentViewController(sb, animated: true, completion: nil)
        }
        else
        {
      
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

