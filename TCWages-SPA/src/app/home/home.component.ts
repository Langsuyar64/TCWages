import { Component, OnInit } from '@angular/core';
import { flattenStyles } from '@angular/platform-browser/src/dom/dom_renderer';
import { HttpClient } from '@angular/common/http';
import { registerModuleFactory } from '@angular/core/src/linker/ng_module_factory_loader';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  registerToggle () {
    this.registerMode = true;
  }

  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }

  cancelRegisterMode(registerMode: boolean){
    this.registerMode = registerMode;
  }

}
