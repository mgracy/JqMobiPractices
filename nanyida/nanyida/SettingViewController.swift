//
//  SettingViewController.swift
//  nanyida
//
//  Created by  马桂新 on 14/12/4.
//  Copyright (c) 2014年 mgracy.blog.163.com. All rights reserved.
//
import UIKit
import Foundation
class SettingViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    
    var dataArray: NSMutableArray?
    var tableView: UITableView?
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        self.view.backgroundColor = UIColor.orangeColor()
        self.navigationItem.title = "您好 Swift"
        
        self.dataArray = NSMutableArray()
        self.dataArray!.addObject("11111")
        self.dataArray!.addObject("22222")
        self.dataArray!.addObject("33333")
        self.dataArray!.addObject("44444")
        
        NSLog("%@", self.dataArray!)
        
        self.tableView = UITableView(frame: self.view.frame, style:UITableViewStyle.Plain)
        self.tableView!.delegate = self
        self.tableView!.dataSource = self
        self.tableView!.registerClass(UITableViewCell.self, forCellReuseIdentifier: "MyTestCell")
        self.view.addSubview(self.tableView!)
        
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    override func viewWillAppear(animated: Bool) // Called when the view is about to made visible. Default does nothing
    {
        super.viewWillAppear(true)
    }
    
    override func viewDidAppear(animated: Bool) // Called when the view has been fully transitioned onto the screen. Default does nothing
    {
        super.viewDidAppear(true)
    }
    
    override func viewWillDisappear(animated: Bool) // Called when the view is dismissed, covered or otherwise hidden. Default does nothing
    {
        super.viewWillDisappear(true)
    }
    
    override func viewDidDisappear(animated: Bool) // Called after the view was dismissed, covered or otherwise hidden. Default does nothing
    {
        super.viewDidDisappear(true)
    }
    
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.dataArray!.count
    }
    
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let cell: UITableViewCell = UITableViewCell(style: UITableViewCellStyle.Subtitle, reuseIdentifier: "MyTestCell")
        cell.textLabel?.text = "Row #  \(self.dataArray!.objectAtIndex(indexPath.row))"
        return cell
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        tableView.deselectRowAtIndexPath(indexPath, animated: true)
        
    }
    
}


