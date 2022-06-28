package unity.plugins.dialog;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.SparseArray;

import com.unity3d.player.UnityPlayer;

public class DialogManager {
    private static DialogManager _instance;

    private int _id;

    /**
     * singleton class
     */
    private DialogManager() {
        _id = 0;
    }

    public static DialogManager getInstance() {
        if(_instance == null) {
            _instance = new DialogManager();
        }
        return _instance;
    }

    public int ShowDialog(String title, String message, String cancel, String confirm, String other){
        ++_id;

        final int id = _id;
        final Activity a = UnityPlayer.currentActivity;
        a.runOnUiThread(new Runnable() {

            public void run() {
                DialogInterface.OnClickListener positiveListener = new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        UnityPlayer.UnitySendMessage("DialogManager", "OnConfirmClick", String.valueOf(id));
                    }
                };

                DialogInterface.OnClickListener negativeListener = new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        UnityPlayer.UnitySendMessage("DialogManager", "OnCancelClick", String.valueOf(id));
                    }
                };

                DialogInterface.OnClickListener neutralListener = new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        UnityPlayer.UnitySendMessage("DialogManager", "OnOtherClick", String.valueOf(id));
                    }
                };

                AlertDialog.Builder dialog = new AlertDialog.Builder(a);
                if (title!=null && !title.equals("")){
                    dialog.setTitle(title);
                }else{
                    dialog.setTitle("");
                }
                if (message!=null && !message.equals("")){
                    dialog.setTitle(message);
                }else{
                    dialog.setTitle("");
                }

                if (confirm!=null && !confirm.equals("")){
                    dialog.setPositiveButton(confirm, positiveListener);
                }
                if (cancel!=null && !cancel.equals("")){
                    dialog.setNegativeButton(cancel, negativeListener);
                }
                if (other!=null && !other.equals("")){
                    dialog.setNeutralButton(other, neutralListener);
                }

                dialog.show();
            }
        });
        return id;
    }

}
