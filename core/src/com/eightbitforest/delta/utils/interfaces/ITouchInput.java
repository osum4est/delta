package com.eightbitforest.delta.utils.interfaces;

/**
 * Created by fjon2248 on 10/22/2015.
 */
public interface ITouchInput {
    void touchDown(int screenX, int screenY, int pointer, int button);

    void touchUp(int screenX, int screenY, int pointer, int button);

    void touchDragged(int screenX, int screenY, int pointer);
}
