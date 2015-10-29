package com.eightbitforest.delta.utils;

import com.eightbitforest.delta.utils.interfaces.ISpawnable;
import com.eightbitforest.delta.utils.interfaces.ITouchInput;
import com.eightbitforest.delta.utils.interfaces.IUpdates;

/**
 * Created by fjon2248 on 10/29/2015.
 */
public class GameRegistry {

    public static void registerObject(Object o) {
        if (o instanceof IUpdates) {
            G.i.updateThese.add((IUpdates) o);
        }

        if (o instanceof ITouchInput) {
            G.i.inputThese.add((ITouchInput) o);
        }
    }

    public static void unregisterObject(Object o) {
        if (o instanceof IUpdates) {
            G.i.updateThese.removeValue((IUpdates) o, true);
        }

        if (o instanceof ITouchInput) {
            G.i.inputThese.removeValue((ITouchInput) o, true);
        }
    }

    public static void registerSpawnable(Class<? extends ISpawnable> spawnable) {
        if (!G.i.spawnThese.contains(spawnable, false)) {
            G.i.spawnThese.add(spawnable);
        }
    }

}
