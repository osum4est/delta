package com.eightbitforest.delta.googleplay;

import com.eightbitforest.delta.iOSGooglePlay;
import org.robovm.apple.foundation.NSError;
import org.robovm.apple.foundation.NSObject;
import org.robovm.apple.uikit.UIApplicationDelegateAdapter;
import org.robovm.apple.uikit.UIView;
import org.robovm.apple.uikit.UIViewController;
import org.robovm.pods.google.games.GPGManager;
import org.robovm.pods.google.games.GPGStatusDelegate;

/**
 * Created by osum4est on 9/19/16.
 */
public class ViewController extends UIViewController implements GPGStatusDelegate {

    public void logIn() {
        GPGManager.getSharedInstance().signIn(iOSGooglePlay.CLIENT_ID, false);
    }

    @Override
    public void didFinishGamesSignIn(NSError error) {

    }

    @Override
    public void didFinishGamesSignOut(NSError error) {

    }

    @Override
    public void didFinishGoogleAuth(NSError error) {

    }

    @Override
    public boolean shouldReauthenticate(NSError error) {
        return false;
    }

    @Override
    public void willReauthenticate(NSError error) {

    }

    @Override
    public void didDisconnect(NSError error) {

    }
}
