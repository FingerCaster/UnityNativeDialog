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

- (int) GetId{
    int id = _id++;
    return id;
}

@end

#if defined(__cplusplus)
extern "C" {
#endif

    typedef void (*cs_callback)(int,int);

    int _ShowDialog(char* title,char* info,char* cancel,char* confirm,char* other,cs_callback callback)
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
        UIAlertController* view = [UIAlertController alertControllerWithTitle:titleStr message:infoStr preferredStyle:UIAlertControllerStyleAlert];
       
        int id = [[DialogManager sharedManager] GetId];
        if (confirm!= NULL) {
            UIAlertAction* ok = [UIAlertAction actionWithTitle:confirmStr
            style:UIAlertActionStyleDefault
            handler:^(UIAlertAction* action)
            {
                callback(id,0);
                [view dismissViewControllerAnimated: YES completion: nil];
            }];
            [view addAction:ok];
        }
        if (cancel!= NULL) {
            UIAlertAction* no = [UIAlertAction actionWithTitle:cancelStr
            style:UIAlertActionStyleDefault
            handler:^(UIAlertAction* action)
            {
                callback(id,1);
                [view dismissViewControllerAnimated: YES completion: nil];
            }];
            [view addAction:no];
        }
        if (other!= NULL) {
            UIAlertAction* otherAction = [UIAlertAction actionWithTitle:otherStr
            style:UIAlertActionStyleDefault
            handler:^(UIAlertAction* action)
            {
                callback(id,2);
                [view dismissViewControllerAnimated: YES completion: nil];
            }];
            [view addAction:otherAction];
        }

        [UnityGetGLViewController() presentViewController: view animated: YES completion: nil];
        
        return id ;
    }
    
#if defined(__cplusplus)
}
#endif
