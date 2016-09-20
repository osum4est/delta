package com.eightbitforest.delta;

import com.eightbitforest.delta.googleplay.ViewController;
import org.robovm.apple.coregraphics.CGRect;
import org.robovm.apple.foundation.NSArray;
import org.robovm.apple.foundation.NSAutoreleasePool;
import org.robovm.apple.foundation.NSDictionary;
import org.robovm.apple.foundation.NSString;
import org.robovm.apple.uikit.*;

import com.badlogic.gdx.backends.iosrobovm.IOSApplication;
import com.badlogic.gdx.backends.iosrobovm.IOSApplicationConfiguration;
import com.eightbitforest.delta.DeltaMain;
import org.robovm.pods.google.games.GPGManager;
import org.robovm.pods.google.games.GPGToastPlacement;

import java.util.ArrayList;

public class IOSLauncher extends IOSApplication.Delegate {
    @Override
    protected IOSApplication createApplication() {


        IOSApplicationConfiguration config = new IOSApplicationConfiguration();
        return new IOSApplication(new DeltaMain(new iOSGooglePlay(this)), config);
    }

    public static void main(String[] argv) {

        NSAutoreleasePool pool = new NSAutoreleasePool();
        UIApplication.main(argv, null, IOSLauncher.class);
        pool.close();
    }
}