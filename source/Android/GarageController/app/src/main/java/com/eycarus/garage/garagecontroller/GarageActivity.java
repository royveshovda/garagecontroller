package com.eycarus.garage.garagecontroller;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;

import org.json.JSONException;
import org.json.JSONObject;


public class GarageActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_garage);

        final Button buttonLeft = (Button) findViewById(R.id.button_left);
        buttonLeft.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                new Worker(1).execute();
            }
        });

        final Button buttonRight = (Button) findViewById(R.id.button_right);
        buttonRight.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                new Worker(2).execute();
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.garage, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.

        boolean handled = true;
        int id = item.getItemId();

        switch(id){
            case R.id.action_settings:
                startActivity(new Intent(this, SettingsActivity.class));
                break;
            default:
                handled = super.onOptionsItemSelected(item);
        }
        return handled;
    }

    private class Worker extends AsyncTask {
        private int _door =0;
        public Worker(int door) {
            _door = door;
        }

        @Override
        protected Object doInBackground(Object... arg0) {
            sendButtonCommand(_door);
            return null;
        }
    }

    private void sendButtonCommand(int buttonNumber) {
        ConnectionFactory factory = new ConnectionFactory();
        factory.setHost("rv-broker.cloudapp.net");
        factory.setUsername("tester");
        factory.setPassword("GoGoTester91234");
        factory.setPort(5672);

        try {
            Connection connection = factory.newConnection();
            Channel channel = connection.createChannel();

            JSONObject message = new JSONObject();
            try {
                message.put("SessionId", "1");
                message.put("DoorNumber", buttonNumber);
                message.put("Signature", "SIGN");
            } catch (JSONException e) {
                runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(GarageActivity.this, "Command FAILED!", Toast.LENGTH_SHORT).show();
                    }
                });
                // TODO Auto-generated catch block
                e.printStackTrace();
            }

            channel.basicPublish("Commands", "",null,message.toString().getBytes());
            channel.close();
            connection.close();
            runOnUiThread(new Runnable() {
                public void run() {
                    Toast.makeText(GarageActivity.this, "Command sent!", Toast.LENGTH_SHORT).show();
                }
            });
        } catch (Exception e) {
            runOnUiThread(new Runnable() {
                public void run() {
                    Toast.makeText(GarageActivity.this, "Command FAILED!", Toast.LENGTH_SHORT).show();
                }
            });
            e.printStackTrace();
        }
    }
}
