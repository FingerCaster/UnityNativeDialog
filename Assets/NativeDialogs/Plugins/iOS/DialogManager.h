@interface DialogManager : NSObject{
    int _id;
}
+ (DialogManager*) sharedManager;
- (int) _ShowDialog:(int)type title:(NSString*)title message:(NSString*)message cancel:(NSString*)cancel confirm:(NSString*)confirm other:(NSString*)other;
@end