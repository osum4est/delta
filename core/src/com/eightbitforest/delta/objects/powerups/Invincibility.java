package com.eightbitforest.delta.objects.powerups;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.generic.Player;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Ids;

public class Invincibility extends Powerup {
    public Invincibility(Level level, float x, float y) {
        super(level, Ids.POWERUP_INVINCIBILITY, x, y, Colors.GREEN);
    }

    @Override
    public void activate(Player player) {
        player.setInvincible(true);
    }
}
