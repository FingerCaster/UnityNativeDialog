//
//  DialogManager.m
//  Unity-iPhone
//
//  Created by super on 2022/6/27.
//

#import <Foundation/Foundation.h>

#import "DialogManager.h"

@implementation DialogManager

static DialogManager * shardDialogManager;

+ (DialogManager*) sharedManager {
    @synchronized(self) {
        if(shardDialogManager == nil) {
            shardDialogManager = [[self alloc]init];
        }
    }
    return shardDialogManager;
}

- (int) _ShowDialog:(int)type title:(NSString*)title message:(NSString*)message cancel:(NSString*)cancel confirm:(NSString*)confirm other:(NSString*)other {
    int id = _id++;
  
    
    UIAlertControllerStyle style = type==0? UIAlertActionStyleDefault: UIAlertControllerStyleAlert;
    UIAlertController* view = [UIAlertController alertControllerWithTitle:title message:message preferredStyle:style];
   
    if (confirm!= NULL) {
        UIAlertAction* ok = [UIAlertAction actionWithTitle:confirm
        style:UIAlertActionStyleDefault
        handler:^(UIAlertAction* action)
        {
            UnitySendMessage("DialogManager","OnConfirmClick",id);
            [view dismissViewControllerAnimated: YES completion: nil];
        }];
        [view addAction:ok];
    }
    if (cancel!= NULL) {
        UIAlertAction* no = [UIAlertAction actionWithTitle:cancel
        style:UIAlertActionStyleDefault
        handler:^(UIAlertAction* action)
        {
            UnitySendMessage("DialogManager","OnCancelClick",id);
            [view dismissViewControllerAnimated: YES completion: nil];
        }];
        [view addAction:no];
    }
    if (other!= NULL) {
        UIAlertAction* otherAction = [UIAlertAction actionWithTitle:other
        style:UIAlertActionStyleDefault
        handler:^(UIAlertAction* action)
        {
            UnitySendMessage("DialogManager","OnOtherClick",id);
            [view dismissViewControllerAnimated: YES completion: nil];
        }];
        [view addAction:otherAction];
    }

    [UnityGetGLViewController() presentViewController: view animated: YES completion: nil];
    return id;
}

@end

#if defined(__cplusplus)
extern "C" {
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
    int _ShowDialog(char* title,char* info,char* cancel,char* confirm,char* other)
    {
        NSString* titleStr ;
        if (title!=NULL && title[0]!='\0') {
            titleStr = [[NSString alloc] initWithUTF8String:title];
        }else{
            titleStr = @"";
        }
        NSString* infoStr ;
        if (info!=NULL && info[0]!='\0') {
            infoStr = [[NSString alloc] initWithUTF8String:info];
        }else{
            infoStr = @"";
        }
        NSString* confirmStr = NULL ;
        if (confirm!= NULL && confirm[0] != '\0') {
            confirmStr = [[NSString alloc] initWithUTF8String:confirm];
        }
        NSString* cancelStr = NULL ;
        if (cancel!= NULL && cancel[0] != '\0') {
            cancelStr = [[NSString alloc] initWithUTF8String:cancel];
        }
        NSString* otherStr = NULL ;
        if (other!= NULL && other[0] != '\0') {
            otherStr = [[NSString alloc] initWithUTF8String:other];
        }
        int type;
        if (confirm!=NULL && cancelStr!=NULL&& otherStr!=NULL) {
            type = 1;
        }else{
            type = 0;
        }
        
        return [[DialogManager sharedManager] _ShowDialog:type title:titleStr message:infoStr cancel:cancelStr confirm:cancelStr other:otherStr];
    }
    
#if defined(__cplusplus)
}
#endif
