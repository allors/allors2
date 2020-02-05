import { AfterViewInit, Component, ElementRef, ViewEncapsulation, Optional, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { RoleField } from 'src/allors/angular';
import Quill, { QuillOptionsStatic } from 'quill';

@Component({
  selector: 'a-mat-quill',
  templateUrl: 'quill.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AllorsMaterialQuillComponent extends RoleField implements AfterViewInit {

  @ViewChild('quill', {static: true})
  quillDiv: ElementRef;

  quill: Quill;

  options: QuillOptionsStatic;

  constructor(
    @Optional() parentForm: NgForm,
  ) {
    super(parentForm);
  }

  ngAfterViewInit() {
    this.options = {
      modules: {
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }, { header: 1 }, { header: 2 }],

          ['bold', 'italic', 'underline', 'strike'],

          ['blockquote'],

          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }],
          [{ indent: '-1' }, { indent: '+1' }],

          [{ color: new Array<any>() }, { background: new Array<any>() }],
          [{ align: new Array<any>() }],

          // ['link', 'image', 'video'],

          ['clean'],
        ]
      },
      placeholder: 'Insert text here ...',
      readOnly: false,
      theme: 'snow',
      scrollingContainer: document.body,
    };

    this.quill = new Quill(this.quillDiv.nativeElement, this.options);

    this.quill.enable(this.disabled || this.canWrite);

    if (this.model) {
      this.quill.pasteHTML(this.model);
    }

    this.quill.on('text-change', () => {
      let html = this.quillDiv.nativeElement.children[0].innerHTML;
      if (html === '<p><br></p>') {
        html = null;
      }

      this.model = html;
    });
  }
}
