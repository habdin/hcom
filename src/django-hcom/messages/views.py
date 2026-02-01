from django.contrib import messages
from django.contrib.messages.views import SuccessMessageMixin
from django.http import HttpResponse


class ReloadSuccessMessageMixin(SuccessMessageMixin):
    def form_valid(self, form):
        self.object = form.save()
        success_message = self.get_success_message(form.cleaned_data)
        if success_message:
            messages.success(self.request, success_message)
        response = HttpResponse(
            self.render_to_response(self.get_context_data(form=form))
        )
        return response

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["messages"] = messages.get_messages(self.request)
        return context
