@interface DialogManager : NSObject{
    int _id;
}
+ (DialogManager*) sharedManager;
- (int) GetId;
@end